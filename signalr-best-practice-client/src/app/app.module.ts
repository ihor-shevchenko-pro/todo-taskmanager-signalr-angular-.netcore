import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AccountComponent } from './components/account/account/account.component';
import { LoginComponent } from './components/account/login/login.component';
import { RegistrationComponent } from './components/account/registration/registration.component';
import { HomeComponent } from './components/home/home.component';
import { ContentComponent } from './components/content/content.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { AccountService } from 'src/core/services/account.service';
import { AuthInterceptor } from 'src/core/auth/auth.guard.spec';
import { NavHeaderComponent } from './components/home/nav-header/nav-header.component';
import { SideMenuComponent } from './components/home/side-menu/side-menu.component';
import { UsersComponent } from './components/content/users/users.component';
import { TodotaskComponent } from './components/content/todotask/todotask.component';
import { CustomDatepickerI18n, I18n } from 'src/core/shared/datepicker/datepicker-i18n';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent,
    ContentComponent,
    NavHeaderComponent,
    SideMenuComponent,
    UsersComponent,
    TodotaskComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      progressBar: true,
      closeButton: true,
    }),
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule,
  ],
  providers: [AccountService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    // define custom NgbDatepickerI18n provider
    I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
