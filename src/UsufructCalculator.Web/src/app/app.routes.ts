import { Routes } from '@angular/router';
import { CalculatorComponent } from './features/calculator/calculator.component';

export const routes: Routes = [
  { path: '', component: CalculatorComponent },
  { path: '**', redirectTo: '' }
];
