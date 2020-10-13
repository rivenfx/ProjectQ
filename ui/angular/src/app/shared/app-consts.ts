export class AppConsts {

  /** 服务端根地址 */
  static remoteServiceUrl = '';
  /** 应用根地址 */
  static appUrl = '';

  /** 应用内url地址 */
  static urls = {
    mainPage: '/admin/dashboard',
    loginPage: '/account/login',
  };

  /** 设置键值 */
  static settings = {
    lang: 'lang',
    token: 'token',
    encryptedToken: 'encryptedToken',
    tokenExpiration: 'tokenExpiration',
  };

  /** 消息 */
  static message = {
    /** 操作成功 */
    success: 'message.operate.successfully',
    /** 操作失败 */
    failure: 'message.operate.failure',
    /** 操作取消 */
    cancelled: 'message.operate.cancelled',
    /** 操作异常 */
    abnormal: 'message.operate.abnormal',
  };


  /** 操作类型 */
  static action = {
    /** 创建 */
    create: 'create',
    /** 编辑 */
    edit: 'edit',
    /** 删除 */
    delete: 'delete',
    /** 浏览 */
    view: 'view'
  };

}
