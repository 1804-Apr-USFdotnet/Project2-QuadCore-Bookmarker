import { Component, OnInit } from '@angular/core';
import { Collection } from '../collection';
import { CollectionService } from '../collection.service'

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css']
})
export class CollectionComponent implements OnInit {

  collectionGroups: Object[] = [];
  collections: Collection[];
  sort: string = "name";
  sortOptions: Object[] = [
    { name: "Name", value: "name:asc" },
    { name: "Rating", value: "rating:desc" }
  ];

  constructor(private collectionSvc: CollectionService) { }

  ngOnInit() {
    this.getCollections();
  }

  getCollections(): void {
    this.collectionSvc.getCollections(null, this.sort)
      .subscribe(
        response => this.makeCollectionGroups(response),
        errors => console.log(errors)
      );
  }

  makeCollectionGroups(collectionList: Collection[]) {
    var group: object[] = [];
    collectionList.forEach(eachObject => {
      group.push(eachObject);
      if (group.length === 3) {
        this.collectionGroups.push(group);
        group = [];
      }
    });
  }
}
