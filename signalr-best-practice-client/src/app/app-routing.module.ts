import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ContentComponent } from './components/content/content.component';
import { AuthGuard } from 'src/core/auth/auth.guard';
import { RegistrationComponent } from './components/account/registration/registration.component';
import { LoginComponent } from './components/account/login/login.component';
import { AccountComponent } from './components/account/account/account.component';
import { UsersComponent } from './components/content/users/users.component';
import { AddTodotaskComponent } from './components/content/todotask/add-todotask/add-todotask.component';
import { SentTodotasksComponent } from './components/content/todotask/sent-todotasks/sent-todotasks.component';
import { ReceivedTodotasksComponent } from './components/content/todotask/received-todotasks/received-todotasks.component';


const routes: Routes = [
  { path: '', redirectTo: '/account/login', pathMatch: 'full' },
  {
    path: 'account', component: AccountComponent,
    children: [
      { path: '', redirectTo: '/account/login', pathMatch: 'full' },
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  {
    path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: ContentComponent,
        children: [
          { path: 'users', component: UsersComponent },
          { path: 'add_todotask/:touserid', component: AddTodotaskComponent },
          { path: 'sent_todotasks', component: SentTodotasksComponent },
          { path: 'received_todotasks', component: ReceivedTodotasksComponent },
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
