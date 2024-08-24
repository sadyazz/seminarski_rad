import {Component, Input, OnInit} from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { TranslocoService } from '@ngneat/transloco';
@Component({
  selector: 'select-language',
  templateUrl: './select-language.component.html',
  styleUrls: ['./select-language.component.css']
})
export class SelectLanguageComponent implements OnInit {
  selectedLanguage: any;
  showDropdown: boolean = false;
  @Input() side:any;

  languages = [
    { name: 'Bosnian', code: 'ba', flag: '../../assets/img/ba.png' },
    { name: 'English', code: 'en', flag: '../../assets/img/en.png' },
    { name: 'German', code: 'de', flag: '../../assets/img/de.png' },
    {name:'French', code:'fr', flag:'../../assets/img/fr.png'}
  ];

  constructor(private translocoService: TranslocoService) {
  }
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  ngOnInit() {
    const savedLang = localStorage.getItem('selectedLang');
    if (savedLang) {
      this.selectedLanguage = this.languages.find(language => language.code === savedLang);
      this.translocoService.setActiveLang(savedLang);
    } else {
      const defaultLang = this.translocoService.getActiveLang();
      this.selectedLanguage = this.languages.find(language => language.code === defaultLang);
      this.translocoService.setActiveLang(defaultLang);
    }
  }



  public changeLanguage(language: any): void {
    this.translocoService.setActiveLang(language.code);
    this.selectedLanguage = language;
    this.showDropdown = false;
    localStorage.setItem('selectedLang', language.code);
  }


}

