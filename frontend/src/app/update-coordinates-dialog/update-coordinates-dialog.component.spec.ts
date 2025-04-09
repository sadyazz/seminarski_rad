import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateCoordinatesDialogComponent } from './update-coordinates-dialog.component';

describe('UpdateCoordinatesDialogComponent', () => {
  let component: UpdateCoordinatesDialogComponent;
  let fixture: ComponentFixture<UpdateCoordinatesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateCoordinatesDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdateCoordinatesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
