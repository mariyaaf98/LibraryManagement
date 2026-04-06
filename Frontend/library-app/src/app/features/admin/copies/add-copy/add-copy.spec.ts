import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCopy } from './add-copy';

describe('AddCopy', () => {
  let component: AddCopy;
  let fixture: ComponentFixture<AddCopy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCopy],
    }).compileComponents();

    fixture = TestBed.createComponent(AddCopy);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
