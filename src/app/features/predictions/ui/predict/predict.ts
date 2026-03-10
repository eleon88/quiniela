import { Component, input } from '@angular/core';

@Component({
  selector: 'app-predict',
  templateUrl: './predict.html',
  styleUrl: './predict.scss',
})
export class PredictComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();
}
