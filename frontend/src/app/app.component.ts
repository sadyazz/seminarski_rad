import {Component, OnInit} from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'frontend';

  constructor(private translocoService: TranslocoService) {}

  ngOnInit() {
    const savedLang = localStorage.getItem('selectedLang');
    if (savedLang) {
      this.translocoService.setActiveLang(savedLang);
    } else {
      const defaultLang = this.translocoService.getActiveLang();
      this.translocoService.setActiveLang(defaultLang);
    }
  }
}
