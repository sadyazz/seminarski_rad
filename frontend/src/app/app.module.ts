import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

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

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    UserProfileComponent,
    PropertyComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([{ path: '', component: HomeComponent },
      {path: 'login', component: LoginComponent},
      {path: 'home', component: HomeComponent},
      {path: 'profile', component: UserProfileComponent, canActivate:[AutorizacijaGuard]},
      {path: 'property', component: PropertyComponent}
    ]),
    AppRoutingModule
  ],
  providers: [
    //provideClientHydration()
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    AutorizacijaGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
