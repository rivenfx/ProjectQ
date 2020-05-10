import { Injectable } from '@angular/core';
import { AppInfoService } from './app-info.service';
import { ACLService } from '@delon/acl';
import { IAppInfo, IClaims } from './interfaces';

@Injectable()
export class PermissionCheckerService {

  constructor(
    private aclService: ACLService,
  ) {
  }


  /**
   * 是否存在权限
   * @param permissionNames 权限/权限集合
   * @param requireAll 校验所有
   */
  isGranted(permissionNames: string | string[], requireAll: boolean = false): boolean {
    if (!permissionNames || permissionNames.length === 0) {
      return true;
    }
    if (!Array.isArray(permissionNames)) {
      permissionNames = [permissionNames];
    }
    return this.aclService.can({
      role: permissionNames,
      mode: requireAll ? 'allOf' : 'oneOf',
    });
  }


  /**
   * 添加权限名称
   * @param permissionName 添加(单个/多个)权限名称
   */
  addPermission(permissionName: string | string[]) {
    if (!permissionName || permissionName.length === 0) {
      return;
    }

    if (Array.isArray(permissionName)) {
      for (let i = 0; i < permissionName.length; i++) {
        const tmpPermissionName = permissionName[i];
        if (
          this.aclService.data.abilities.find(
            item => item === tmpPermissionName,
          )
        ) {
          continue;
        }
        this.aclService.attachRole([tmpPermissionName]);
      }
    } else {
      if (
        this.aclService.data.abilities.find(item => item === permissionName)
      ) {
        return;
      }
      this.aclService.attachRole([permissionName]);
    }
  }

  /**
   * 移除单个/多个权限
   * @param permissionName (单个/多个)权限名称
   */
  removePermission(permissionName: string | string[]) {
    if (!permissionName || permissionName.length === 0) {
      return;
    }

    if (Array.isArray(permissionName)) {
      this.aclService.removeRole(permissionName);
    } else {
      this.aclService.removeRole([permissionName]);
    }
  }

  /**
   * 清空所有权限
   */
  clear() {
    const tmpPermissionNames: any = this.aclService.data.abilities;
    this.removePermission(tmpPermissionNames);
  }

  /**
   * 填充数据
   * @param input
   */
  extend(input: IClaims) {
    const permissions: string[] = [];
    for (const permission in input.grantedClaims) {
      permissions.push(permission);
    }
    this.addPermission(permissions);
  }
}
