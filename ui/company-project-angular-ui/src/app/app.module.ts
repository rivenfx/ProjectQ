import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';

//
import { StartupService, I18nCommon, I18NService, I18nLoader } from '@core';

//
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { GlobalConfigModule } from './global-config.module';
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from '@shared';
import { STWidgetModule } from './shared/st-widget/st-widget.module';
import { AppRoutingModule } from './app-routing.module';
import { ServiceProxyModule } from './service-proxies';
import { RivenModule } from './shared/riven';


// #region default language

registerLocaleData(I18nCommon.DEFAULT_LANG.ng, I18nCommon.DEFAULT_LANG.abbr);
const I18N_SERVICE_PROVIDES = [
  { provide: ALAIN_I18N_TOKEN, useClass: I18NService, multi: false },
];
// #region

// #region JSON Schema form (using @delon/form)
import { JsonSchemaModule } from '@shared';
import { ALAIN_I18N_TOKEN } from '@delon/theme';

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
    CoreModule,
    SharedModule,
    LayoutModule,
    //
    STWidgetModule,
    //
    AppRoutingModule,
    //
    ...I18nCommon.I18NSERVICE_MODULES,
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
