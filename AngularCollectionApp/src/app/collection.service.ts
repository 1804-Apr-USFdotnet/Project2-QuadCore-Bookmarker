import { Injectable } from '@angular/core';
import { Collection } from './collection';
import { Observable, of } from 'rxjs';
import { map, catchError, tap, filter, flatMap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CollectionService {

  constructor(private messageSvc: MessageService,
  private http: HttpClient) { }

  private collectionsUrl = 'http://ec2-18-188-153-64.us-east-2.compute.amazonaws.com/Bookmarker/api/Collections';
  private usersUrl = 'http://ec2-18-188-153-64.us-east-2.compute.amazonaws.com/Bookmarker/api/Users/';

  getCollections(search?: string, sort?: string): Observable<Collection[]> {
    this.messageSvc.add("CollectionSvc: fetched collections")

    var queryString = "";
    if (search && !sort) {
      queryString += "?search=" + search;
    }
    else if (!search && sort) {
      queryString += "?sort=" + sort;
    }
    else if (!search && !sort) {
      queryString += "?search=" + search + "&sort=" + sort;
    }

    var x = this.http.get<Collection[]>(this.collectionsUrl + queryString);
    return x;
  }

  getCollectionById(id : string): Observable<Collection> {
    var url = `${this.collectionsUrl}/${id}`;

    var x = this.http.get<Collection>(url).pipe(
      tap(_ => this.messageSvc.add(`CollectionSvc: fetched collection id=${id}`))
    );
    return x;
  }

  getCollectionsByUserId(id : string): Observable<Collection[]> {
    var url = `${this.usersUrl}/${id}/collections`;

    var x = this.http.get<Collection[]>(url).pipe(
      tap(_ => this.messageSvc.add(`CollectionSvc: fetched collections for user id=${id}`))
    );
    return x;
  }
}
