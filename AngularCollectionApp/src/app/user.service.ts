import { Injectable } from '@angular/core';
import { User } from './user';
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

  private usersUrl = 'http://ec2-18-188-153-64.us-east-2.compute.amazonaws.com/Bookmarker/api/Users';

  getUsers(search?: string, sort?: string): Observable<User[]> {
    this.messageSvc.add("UserSvc: fetched users")

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

    var x = this.http.get<User[]>(this.usersUrl + queryString);
    return x;
  }

  getUserById(id : string): Observable<User> {
    var url = `${this.usersUrl}/${id}`;

    var x = this.http.get<User>(url).pipe(
      tap(_ => this.messageSvc.add(`UserSvc: fetched user id=${id}`))
    );
    return x;
  }

}
