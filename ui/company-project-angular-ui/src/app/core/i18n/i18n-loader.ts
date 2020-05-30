import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { SessionService } from '../../shared/riven';

export class I18nLoader extends TranslateLoader {

  constructor(
    private sessionSer: SessionService,
  ) {
    super();
  }


  getTranslation(lang: string): Observable<any> {
    return this.sessionSer.loadLocalization();
  }

}
