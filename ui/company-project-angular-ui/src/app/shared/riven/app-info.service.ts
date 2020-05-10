import { Injectable } from '@angular/core';
import { IAppInfo } from './interfaces';

@Injectable()
export class AppInfoService {

  private _current: IAppInfo;

  /** 当前app配置信息 */
  get current(): IAppInfo {
    return this._current;
  }

  /** 获取配置值 */
  getSettingValue<T>(name: string): T {
    return this.current.settings[name] as T;
  }

  /** 设置应用配置信息 */
  setAppInfo(input: IAppInfo) {
    this._current = input;
  }

  /**
   * 加载或更新AppInfo
   */
  loadOrUpdateAppInfo() {

  }
}
