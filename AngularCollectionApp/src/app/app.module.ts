import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'

import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutingModule } from './/app-routing.module';
import { CollectionComponent } from './collection/collection.component';
import { CollectionDetailComponent } from './collection-detail/collection-detail.component';
import { BookmarksComponent } from './bookmarks/bookmarks.component'

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    UserDetailComponent,
    MessagesComponent,
    CollectionComponent,
    CollectionDetailComponent,
    BookmarksComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
