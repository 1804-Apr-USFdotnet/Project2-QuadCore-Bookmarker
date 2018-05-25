// import { Injectable } from '@angular/core';

// @Injectable({
//   providedIn: 'root'
// })
// export class CollectionService {

//   constructor() { }
// }


import { Injectable } from '@angular/core';
import { Collection } from './collection';
//import { USERS } from './mock-users';
import { Observable, of } from 'rxjs';
import { map, catchError, tap, filter, flatMap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private messageSvc: MessageService,
  private http: HttpClient) { }

  private collectionsUrl = 'http://ec2-18-188-153-64.us-east-2.compute.amazonaws.com/Bookmarker/api/Collections';

  getCollections(search?: string, sort?: string): Observable<Collection[]> {
    this.messageSvc.add("UserSvc: fetched collections")
    
    var queryString = "";
    if (search && !sort) {
      queryString += "?search=" + search;
    }
    else if (!search && sort) {
      queryString += "?sort" + sort;
    }
    else if (!search && !sort) {
      queryString += "?search=" + search + "&sort=" + sort;
    }

    var x = this.http.get<Collection[]>(this.collectionsUrl + queryString);
    return x;
  }
}
