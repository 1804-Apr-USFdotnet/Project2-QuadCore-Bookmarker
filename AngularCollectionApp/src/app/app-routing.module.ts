import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user/user.component'
import { UserDetailComponent } from './user-detail/user-detail.component';
import { CollectionComponent } from './collection/collection.component'
import { CollectionDetailComponent } from './collection-detail/collection-detail.component';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
  { path: '', redirectTo: '/collections', pathMatch: 'full' },
  { path: 'users', component: UserComponent },
  { path: 'detail/:id', component: UserDetailComponent },
  { path: 'collections', component: CollectionComponent },
  { path: 'collections/:id', component: CollectionDetailComponent }
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
