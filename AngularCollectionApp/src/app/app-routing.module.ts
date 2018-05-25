import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user/user.component'
import { UserDetailComponent } from './user-detail/user-detail.component';
import { CollectionComponent } from './collection/collection.component'

const routes: Routes = [
  { path: '', redirectTo: '/collections', pathMatch: 'full' },
  { path: 'users', component: UserComponent },
  { path: 'detail/:username', component: UserDetailComponent },
  { path: 'collections', component: CollectionComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
