import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
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
import { AddTodotaskComponent } from './components/content/todotask/add-todotask/add-todotask.component';
import { SentTodotasksComponent } from './components/content/todotask/sent-todotasks/sent-todotasks.component';
import { ReceivedTodotasksComponent } from './components/content/todotask/received-todotasks/received-todotasks.component';

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
    AddTodotaskComponent,
    SentTodotasksComponent,
    ReceivedTodotasksComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      progressBar: true,
      closeButton: true,
    }),
    AppRoutingModule,
    ReactiveFormsModule.withConfig({warnOnNgModelWithFormControl: 'never'}),
    HttpClientModule,
    NgbModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  providers: [AccountService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
