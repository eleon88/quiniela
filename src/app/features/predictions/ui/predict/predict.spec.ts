import { TestBed } from '@angular/core/testing';
import { PredictComponent } from './predict';

describe('PredictComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PredictComponent],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(PredictComponent);
    fixture.componentRef.setInput('boardId', 'board-1');
    fixture.componentRef.setInput('roundId', 'round-1');
    expect(fixture.componentInstance).toBeTruthy();
  });
});
