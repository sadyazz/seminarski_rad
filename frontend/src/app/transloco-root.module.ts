
import { HttpClient } from '@angular/common/http';
import {
  TRANSLOCO_LOADER,
  Translation,
  TranslocoLoader,
  TRANSLOCO_CONFIG,
  translocoConfig,
  TranslocoModule, provideTransloco
} from '@ngneat/transloco';
import { Injectable, isDevMode, NgModule } from '@angular/core';
import {TranslocoHttpLoader} from "./transloco-loader";


//@Injectable({ providedIn: 'root' })
//export class TranslocoHttpLoader implements TranslocoLoader {
  //constructor(private http: HttpClient) {}

 // getTranslation(lang: string) {
  //  return this.http.get<Translation>(`/assets/i18n/${lang}.json`);
  //}
//}
@NgModule({
  exports: [ TranslocoModule ],
  providers: [
      provideTransloco({
        config: {
          availableLangs: ['ba', 'en', 'de', 'fr'],
          defaultLang: 'en',
          // Remove this option if your application doesn't support changing language in runtime.
          reRenderOnLangChange: true,
          prodMode: !isDevMode(),
        },
        loader: TranslocoHttpLoader
      }),
  ],
})
export class TranslocoRootModule {}
