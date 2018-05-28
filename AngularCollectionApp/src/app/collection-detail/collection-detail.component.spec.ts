import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectionDetailComponent } from './collection-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientModule } from '@angular/common/http';
import { BookmarksComponent } from '../bookmarks/bookmarks.component';

describe('CollectionDetailComponent', () => {
  let component: CollectionDetailComponent;
  let fixture: ComponentFixture<CollectionDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations:
      [
        CollectionDetailComponent,
        BookmarksComponent
      ],
      imports: [
        RouterTestingModule, HttpClientModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CollectionDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
