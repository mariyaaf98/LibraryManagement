import { TestBed } from '@angular/core/testing';

import { Copy } from './copy';

describe('Copy', () => {
  let service: Copy;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Copy);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
