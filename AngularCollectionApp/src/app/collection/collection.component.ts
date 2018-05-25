import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { CollectionService } from '../collection.service'

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit {

  collections: Collection[];
  sort: string;
  sortOptions: Object[] = [
    { name: "Name", value: "name:asc" },
    { name: "Rating", value: "rating:desc" }
  ];

  constructor(private collectionSvc: CollectionService) { }

  ngOnInit() {
    this.getCollections(this.sort);
  }

  getCollections(sortQ: string): void {
    this.collectionSvc.getCollections(null, "name:desc")
      .subscribe(
        response => this.collections = response,
        errors => console.log(errors)
      );
  }
}
