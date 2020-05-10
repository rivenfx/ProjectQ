import { Injectable } from '@angular/core';

@Injectable()
export class AuthService {

  private _token: string;

  get token(): string {
    return this._token;
  }

  setToken(input: string) {
    this._token = input;
  }

}
