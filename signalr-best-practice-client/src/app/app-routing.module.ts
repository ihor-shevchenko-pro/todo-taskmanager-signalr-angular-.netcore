import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ContentComponent } from './components/content/content.component';
import { AuthGuard } from 'src/core/auth/auth.guard';
import { RegistrationComponent } from './components/account/registration/registration.component';
import { LoginComponent } from './components/account/login/login.component';
import { AccountComponent } from './components/account/account/account.component';
import { UsersComponent } from './components/content/users/users.component';
import { TodotaskComponent } from './components/content/todotask/todotask.component';


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
          { path: 'todotask', component: TodotaskComponent },
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
