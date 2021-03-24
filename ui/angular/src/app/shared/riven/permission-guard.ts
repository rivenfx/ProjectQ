import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Data,
  Route,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { ACLCanType, ACLService } from '@delon/acl';
import { AlainACLConfig } from '@delon/util';
import { SessionService } from '@shared/riven/session.service';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })
export class PermissionGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private srv: ACLService,
    private router: Router,
    private sessionSrv: SessionService,
  ) {
  }

  private process(data?: Data): boolean {
    const { session } = this.sessionSrv;
    data = {
      guard_url: this.srv.guard_url,
      ...data,
    };

    // 未登录返回 false
    if (!session.auth || !session.auth.userId) {
      this.router.navigateByUrl(data.guard_url);
      return false;
    }

    // 已登录但路由未配置验证权限返回 true
    if (!data.permissions) {
      return true;
    }

    // 有权限则进行判断，
    if (Array.isArray(data.permissions)) {
      for (const permission of data.permissions) {
        if (!this.srv.can(permission)) {
          if (data.mode === 'allOf') {
            this.router.navigateByUrl(data.guard_url);
            return false;
          }
        } else {
          if (data.mode !== 'allOf') {
            return true;
          }
        }
      }
      return true;
    }

    if (this.srv.can(data.permissions)) {
      return true;
    }
    this.router.navigateByUrl(data.guard_url);
    return false;
  }

  // lazy loading
  canLoad(route: Route): boolean {
    return this.process(route.data!);
  }

  // all children route
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(childRoute, state);
  }

  // route
  canActivate(route: ActivatedRouteSnapshot, _state: RouterStateSnapshot | null): boolean {
    return this.process(route.data);
  }
}
