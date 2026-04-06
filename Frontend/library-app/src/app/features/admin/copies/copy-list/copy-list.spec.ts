import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyList } from './copy-list';

describe('CopyList', () => {
  let component: CopyList;
  let fixture: ComponentFixture<CopyList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CopyList],
    }).compileComponents();

    fixture = TestBed.createComponent(CopyList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
