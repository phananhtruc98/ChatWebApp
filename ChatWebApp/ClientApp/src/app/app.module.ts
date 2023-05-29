import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './account/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './account/login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { AuthInterceptor } from './_helpers/auth.interceptor';
import { ImagekitioAngularModule } from 'imagekitio-angular';
import { environment } from 'src/environments/environment';
import { SuggestionsComponent } from './pages/suggestions/suggestions.component';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ImagekitioAngularModule.forRoot({
      publicKey: environment.publicKey,
      urlEndpoint: environment.urlEndpoint,
    }),
  ],
  declarations: [
    AppComponent,
    RegisterComponent,
    HomeComponent,
    LoginComponent,
    ProfileComponent,
    ContactsComponent,
    SuggestionsComponent,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class AppModule {}
