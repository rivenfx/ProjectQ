import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';

//
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { StartupService, I18nCommon, I18nService, I18nModule } from '@core';

//
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { GlobalConfigModule } from './global-config.module';
import { LayoutModule } from './layout/layout.module';
import { SharedModule, STWidgetModule, RivenModule, JsonSchemaModule } from '@shared';
import { AppRoutingModule } from './app-routing.module';
import { ServiceProxyModule } from './service-proxies';


// #region default language

registerLocaleData(I18nCommon.DEFAULT_LANG.ng, I18nCommon.DEFAULT_LANG.abbr);
const I18N_SERVICE_PROVIDES = [
  { provide: ALAIN_I18N_TOKEN, useClass: I18nService, multi: false },
];
// #region

// #region JSON Schema form (using @delon/form)


const FORM_MODULES = [JsonSchemaModule];


// global third module
const GLOBAL_THIRD_MODULES = [
  ServiceProxyModule.forRoot(),
  RivenModule.forRoot(),
];


export function StartupServiceFactory(startupService: StartupService) {
  return () => startupService.load();
}

const APPINIT_PROVIDES = [
  StartupService,
  {
    provide: APP_INITIALIZER,
    useFactory: StartupServiceFactory,
    deps: [StartupService],
    multi: true,
  },
];

// #endregion


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    //
    GlobalConfigModule.forRoot(),
    I18nModule.forRoot(),
    CoreModule,
    SharedModule,
    LayoutModule,
    //
    STWidgetModule,
    //
    AppRoutingModule,
    //
    ...FORM_MODULES,
    ...GLOBAL_THIRD_MODULES,
  ],
  providers: [
    ...I18nCommon.LANG_PROVIDES,
    ...I18N_SERVICE_PROVIDES,
    ...APPINIT_PROVIDES,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
