import { AxiosInstance } from "axios";
import { Type } from "../core";


export class ServiceProxyFactory {

  /** 服务端地址 */
  baseUrl = '';

  /** axios实例 */
  axiosInstance: AxiosInstance | undefined = undefined;

  get<T>(serviceType: Type<T>, baseUrl?: string, instance?: AxiosInstance): T | undefined {
    return new serviceType(baseUrl, instance);
  }

}

const serviceFactory: ServiceProxyFactory = new ServiceProxyFactory();
export default serviceFactory;
