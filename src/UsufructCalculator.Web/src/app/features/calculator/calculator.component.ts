import { Component, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UsufructService } from '../../shared/services/usufruct.service';
import { Gender, CalculationMethod } from '../../shared/models/usufruct.models';

@Component({
  selector: 'app-calculator',
  imports: [FormsModule],
  templateUrl: './calculator.component.html',
  styleUrl: './calculator.component.css'
})
export class CalculatorComponent {
  private readonly usufructService = inject(UsufructService);

  protected readonly title = signal('Vruchtgebruik berekening');

  // Form fields
  protected age = signal('');
  protected propertyValue = signal('');
  protected gender = signal('');
  protected calculationMethod = signal<CalculationMethod>(CalculationMethod.EenLeven);

  // Validation error messages
  protected ageError = signal('');
  protected propertyValueError = signal('');
  protected genderError = signal('');

  // Result signals
  protected calculatedValue = signal<number | null>(null);
  protected factor = signal<number | null>(null);
  protected isLoading = signal(false);
  protected apiError = signal('');

  protected validateAge(required = false): void {
    const ageValue = Number(this.age());
    if (this.age() === '') {
      this.ageError.set(required ? 'Leeftijd is verplicht' : '');
    } else if (isNaN(ageValue) || ageValue < 0 || ageValue > 120) {
      this.ageError.set('Leeftijd moet tussen 0 en 120 zijn');
    } else {
      this.ageError.set('');
    }
  }

  protected validatePropertyValue(required = false): void {
    const value = Number(this.propertyValue());
    if (this.propertyValue() === '') {
      this.propertyValueError.set(required ? 'Eigendom waarde is verplicht' : '');
    } else if (isNaN(value) || value < 0) {
      this.propertyValueError.set('Eigendom waarde moet positief zijn');
    } else if (!Number.isInteger(value)) {
      this.propertyValueError.set('Eigendom waarde moet in hele euro\'s zijn');
    } else {
      this.propertyValueError.set('');
    }
  }

  protected calculate(): void {
    // Validate all fields (required=true)
    this.validateAge(true);
    this.validatePropertyValue(true);

    // Validate gender
    if (!this.gender()) {
      this.genderError.set('Selecteer een geslacht');
    } else {
      this.genderError.set('');
    }

    // Check if there are any validation errors
    if (this.ageError() || this.propertyValueError() || this.genderError()) {
      return;
    }

    // Clear previous results and errors
    this.apiError.set('');
    this.isLoading.set(true);

    // Make API call
    const request = {
      amount: Number(this.propertyValue()),
      age: Number(this.age()),
      gender: this.gender() === 'male' ? Gender.Male : Gender.Female,
      calculationMethod: this.calculationMethod()
    };

    this.usufructService.calculate(request).subscribe({
      next: (response) => {
        this.calculatedValue.set(response.calculatedValue);
        this.factor.set(response.factor);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('API error:', error);
        this.apiError.set('Er is een fout opgetreden bij het berekenen. Probeer het later opnieuw.');
        this.isLoading.set(false);
      }
    });
  }

  // Enum for template
  protected readonly CalculationMethod = CalculationMethod;
  protected readonly calculationMethods = [
    { value: CalculationMethod.EenLeven, label: 'Eén Leven' }
  ];
  protected readonly genders = [
    { value: 'male', label: 'Man' },
    { value: 'female', label: 'Vrouw' }
  ];

  protected readonly Gender = Gender;
}
