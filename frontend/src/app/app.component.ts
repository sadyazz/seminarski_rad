import {Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import {isPlatformBrowser} from "@angular/common";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'frontend';

  constructor(private translocoService: TranslocoService, @Inject(PLATFORM_ID) private platformId: Object) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
    const savedLang = localStorage.getItem('selectedLang');
    if (savedLang) {
      this.translocoService.setActiveLang(savedLang);
    } else {
      const defaultLang = this.translocoService.getActiveLang();
      this.translocoService.setActiveLang(defaultLang);
    }
  }}
}
