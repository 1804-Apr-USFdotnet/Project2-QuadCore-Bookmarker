import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.css']
})
export class BookmarksComponent implements OnInit {
  @Input() url: string;
  bookmarks : object;

  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.getBookmarks();
  }

  getBookmarks(): void {
    this.httpClient.get(this.url).subscribe(
      response => this.bookmarks = response,
      errors => console.log(errors)
    );
  }

}
