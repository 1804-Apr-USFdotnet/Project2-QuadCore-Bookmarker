import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.css']
})
export class BookmarksComponent implements OnInit {
  @Input() url: string;

  constructor() { }

  ngOnInit() {
  }

  getBookmarks(): void {

  }

}
