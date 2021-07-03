import { Injectable, Injector } from '@angular/core';
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
import { ACLService } from '@delon/acl';
import {
  IPermissionCheckerService,
  PERMISSION_CHECKER_SER,
  IRivenCommonConfig,
  RIVEN_COMMON_CONFIG,
} from '@rivenfx/ng-common';
import { SessionService } from '@shared/riven/session.service';


@Injectable({ providedIn: 'root' })
export class PermissionGuard implements CanActivate, CanActivateChild, CanLoad {

  permissionCheckerSer: IPermissionCheckerService;

  config: IRivenCommonConfig;

  constructor(
    injector: Injector,
    public router: Router,
    public sessionSrv: SessionService,
  ) {
    this.permissionCheckerSer = injector.get<IPermissionCheckerService>(PERMISSION_CHECKER_SER);
    this.config = injector.get<IRivenCommonConfig>(RIVEN_COMMON_CONFIG);
  }

  private process(data?: Data): boolean {
    const { session } = this.sessionSrv;
    data = {
      loginPage: this.config.routes.loginPage,
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
    if (data.mode === 'allOf') {
      if (!this.permissionCheckerSer.isGranted(data.permissions)) {
        this.router.navigateByUrl(data.guard_url);
        return false;
      }
    } else {
      if (!this.permissionCheckerSer.isGrantedAny(data.permissions)) {
        this.router.navigateByUrl(data.guard_url);
        return false;
      }
    }

    return true;
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
