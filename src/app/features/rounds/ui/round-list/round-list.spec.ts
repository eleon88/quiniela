import { TestBed } from '@angular/core/testing';
import { RoundListComponent } from './round-list';

describe('RoundListComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoundListComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(RoundListComponent);
    fixture.componentRef.setInput('boardId', 'test-id');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
