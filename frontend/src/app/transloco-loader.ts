/*
import { inject, Injectable } from "@angular/core";
import { Translation, TranslocoLoader } from "@ngneat/transloco";
import { HttpClient } from "@angular/common/http";
import {environment} from "../environments/environment";

@Injectable({ providedIn: 'root' })
export class TranslocoHttpLoader implements TranslocoLoader {
    private http = inject(HttpClient);

    //getTranslation(lang: string) {
    //    return this.http.get<Translation>(`${environment.baseUrl}/assets/i18n/${lang}.json`);
    //}
  getTranslation(langPath: string) {
    return fetch(`/assets/i18n/${langPath}.json`).then<Translation>((res) => res.json());
  }
}*/
import { HttpClient } from "@angular/common/http";
import {
  Translation,
  TRANSLOCO_LOADER,
  TranslocoLoader
} from "@ngneat/transloco";
import { Injectable } from "@angular/core";
import { environment } from "../environments/environment";

@Injectable({ providedIn: "root" })
export class TranslocoHttpLoader implements TranslocoLoader {
  constructor(private http: HttpClient) {}

  /*getTranslation(lang: string) {
    return this.http.get<Translation>(
      `${environment.baseUrl}/assets/i18n/${lang}.json`
    );
  }*/
  /*getTranslation(lang: string) {
    return this.http.get<Translation>(`/assets/i18n/${lang}.json`);
  }*/
  getTranslation(langPath: string) {
    return fetch(`/assets/i18n/${langPath}.json`).then<Translation>((res) => res.json());
  }
}

export const translocoLoader = {
  provide: TRANSLOCO_LOADER,
  useClass: TranslocoHttpLoader
};
