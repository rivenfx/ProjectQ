export declare const Type: FunctionConstructor;

export declare interface Type<T> extends Function {
  new(...args: any[]): T;
}
