import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  Data,
} from '@angular/router';
import { of, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ACLCanType, ACLService } from '@delon/acl';
import { AlainACLConfig } from '@delon/util';


@Injectable({ providedIn: 'root' })
export class ClaimsGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private srv: ACLService, private router: Router) {
  }

  private process(data: Data): boolean {
    data = {
      guard_url: this.srv.guard_url,
      ...data,
    };

    if (!data.claims) {
      return true;
    }
    if (Array.isArray(data.claims)) {
      for (let i = 0; i < data.claims.length; i++) {
        if (!this.srv.can(data.claims[i])) {
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

    if (this.srv.can(data.claims)) {
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
