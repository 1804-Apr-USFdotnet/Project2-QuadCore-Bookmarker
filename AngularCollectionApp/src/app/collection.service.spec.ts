import { TestBed, inject } from '@angular/core/testing';

import { CollectionService } from './collection.service';
import { HttpClientModule } from '@angular/common/http';

describe('CollectionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CollectionService],
      imports: [HttpClientModule]
    });
  });

  it('should be created', inject([CollectionService], (service: CollectionService) => {
    expect(service).toBeTruthy();
  }));
});
