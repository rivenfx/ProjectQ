export class LazyInit<T>{
  private _factory: () => T;
  private _instance: T;

  constructor(factory: () => T) {
    this._factory = factory;
  }

  get instance(): T {
    if (this._instance) {
      return this._instance;
    }

    this._instance = this._factory();
  }
}
