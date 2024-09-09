import { NgModule } from '@angular/core';
import { BrowserModule} from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {RouterModule} from '@angular/router';

import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import {AuthInterceptor} from "../helpers/auth/auth-interceptor.service";
import {AutorizacijaGuard} from "../helpers/auth/autorizacija-guard.service";
import { PropertyComponent } from './property/property.component';
import { TwofaPageComponent } from './twofa-page/twofa-page.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import {MatDivider} from "@angular/material/divider";
import {MatIcon} from "@angular/material/icon";
import {MatCard, MatCardContent} from "@angular/material/card";
import { SelectLanguageComponent } from './components/select-language/select-language.component';
import { TranslocoRootModule } from './transloco-root.module';
import {translate} from '@ngneat/transloco'


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    UserProfileComponent,
    PropertyComponent,
    TwofaPageComponent,
    SelectLanguageComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([{path: '', component: HomeComponent},
      {path: 'login', component: LoginComponent},
      {path: 'home', component: HomeComponent},
      {path: 'profile', component: UserProfileComponent, canActivate: [AutorizacijaGuard]},
      {path: 'property/:id', component: PropertyComponent},
      {path: 'twofaPage', component: TwofaPageComponent}
    ]),
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatInputModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatDivider,
    MatIcon,
    MatCardContent,
    MatCard,
    TranslocoRootModule
  ],
  providers: [
    //provideClientHydration()
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    AutorizacijaGuard,
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
