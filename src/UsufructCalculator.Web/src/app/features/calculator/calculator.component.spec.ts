import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { CalculatorComponent } from './calculator.component';
import { Gender, CalculationMethod } from '../../shared/models/usufruct.models';
import { environment } from '../../../environments/environment';

describe('CalculatorComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalculatorComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  it('should create the component', () => {
    const fixture = TestBed.createComponent(CalculatorComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(CalculatorComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Vruchtgebruik berekening');
  });

  describe('Validation', () => {
    it('should show error when age is below 0', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('-1');
      component['validateAge'](true);

      expect(component['ageError']()).toBe('Leeftijd moet tussen 0 en 120 zijn');
    });

    it('should show error when age is above 120', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('121');
      component['validateAge'](true);

      expect(component['ageError']()).toBe('Leeftijd moet tussen 0 en 120 zijn');
    });

    it('should show error when age is empty and required', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('');
      component['validateAge'](true);

      expect(component['ageError']()).toBe('Leeftijd is verplicht');
    });

    it('should not show error when age is empty and not required', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('');
      component['validateAge'](false);

      expect(component['ageError']()).toBe('');
    });

    it('should show error when property value is negative', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['propertyValue'].set('-100');
      component['validatePropertyValue'](true);

      expect(component['propertyValueError']()).toBe('Eigendom waarde moet positief zijn');
    });

    it('should show error when property value is not a whole number', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['propertyValue'].set('100.50');
      component['validatePropertyValue'](true);

      expect(component['propertyValueError']()).toBe('Eigendom waarde moet in hele euro\'s zijn');
    });

    it('should show error when property value is empty and required', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['propertyValue'].set('');
      component['validatePropertyValue'](true);

      expect(component['propertyValueError']()).toBe('Eigendom waarde is verplicht');
    });

    it('should accept valid age', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('50');
      component['validateAge'](true);

      expect(component['ageError']()).toBe('');
    });

    it('should accept valid property value', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['propertyValue'].set('100000');
      component['validatePropertyValue'](true);

      expect(component['propertyValueError']()).toBe('');
    });
  });

  describe('Calculate', () => {
    it('should validate all fields when calculate is called', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['calculate']();

      expect(component['ageError']()).toBe('Leeftijd is verplicht');
      expect(component['propertyValueError']()).toBe('Eigendom waarde is verplicht');
      expect(component['genderError']()).toBe('Selecteer een geslacht');
    });

    it('should not call API when validation fails', () => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;
      const httpMock = TestBed.inject(HttpTestingController);

      component['age'].set('');
      component['calculate']();

      httpMock.expectNone(`${environment.apiUrl}/v1/usufruct-calculations`);
      httpMock.verify();
    });
  });

  describe('Integration Tests', () => {
    let httpMock: HttpTestingController;

    beforeEach(() => {
      httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
      httpMock.verify();
    });

    it('should successfully calculate usufruct value with valid inputs', (done) => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      // Set valid form values
      component['age'].set('50');
      component['propertyValue'].set('100000');
      component['gender'].set('male');
      component['calculationMethod'].set(CalculationMethod.EenLeven);

      // Trigger calculation
      component['calculate']();

      // Expect loading state
      expect(component['isLoading']()).toBe(true);

      // Mock API response
      const req = httpMock.expectOne((request) => {
        return request.url === `${environment.apiUrl}/v1/usufruct-calculations` &&
               request.params.get('amount') === '100000' &&
               request.params.get('age') === '50' &&
               request.params.get('gender') === '0';
      });

      expect(req.request.method).toBe('GET');

      const mockResponse = {
        originalValue: 100000,
        factor: 0.56,
        calculatedValue: 56000
      };

      req.flush(mockResponse);

      // Wait for async operations
      setTimeout(() => {
        expect(component['isLoading']()).toBe(false);
        expect(component['calculatedValue']()).toBe(56000);
        expect(component['factor']()).toBe(0.56);
        expect(component['apiError']()).toBe('');
        done();
      }, 0);
    });

    it('should handle API error correctly', (done) => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      // Set valid form values
      component['age'].set('60');
      component['propertyValue'].set('200000');
      component['gender'].set('female');

      // Trigger calculation
      component['calculate']();

      expect(component['isLoading']()).toBe(true);

      // Mock API error
      const req = httpMock.expectOne((request) => {
        return request.url === `${environment.apiUrl}/v1/usufruct-calculations`;
      });

      req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });

      // Wait for async operations
      setTimeout(() => {
        expect(component['isLoading']()).toBe(false);
        expect(component['apiError']()).toBe('Er is een fout opgetreden bij het berekenen. Probeer het later opnieuw.');
        expect(component['calculatedValue']()).toBeNull();
        done();
      }, 0);
    });

    it('should clear previous results and errors when calculating', (done) => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      // Set previous state
      component['calculatedValue'].set(50000);
      component['apiError'].set('Previous error');

      // Set valid form values
      component['age'].set('50');
      component['propertyValue'].set('100000');
      component['gender'].set('male');

      // Trigger calculation
      component['calculate']();

      // Errors should be cleared immediately
      expect(component['apiError']()).toBe('');

      const req = httpMock.expectOne((request) => {
        return request.url === `${environment.apiUrl}/v1/usufruct-calculations`;
      });

      req.flush({
        originalValue: 100000,
        factor: 0.56,
        calculatedValue: 56000
      });

      setTimeout(() => {
        expect(component['calculatedValue']()).toBe(56000);
        done();
      }, 0);
    });

    it('should map gender correctly for female', (done) => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('50');
      component['propertyValue'].set('100000');
      component['gender'].set('female');

      component['calculate']();

      const req = httpMock.expectOne((request) => {
        return request.url === `${environment.apiUrl}/v1/usufruct-calculations` &&
               request.params.get('gender') === '1'; // Female = 1
      });

      req.flush({
        originalValue: 100000,
        factor: 0.60,
        calculatedValue: 60000
      });

      setTimeout(() => {
        expect(component['calculatedValue']()).toBe(60000);
        done();
      }, 0);
    });

    it('should toggle loading state correctly during API call', (done) => {
      const fixture = TestBed.createComponent(CalculatorComponent);
      const component = fixture.componentInstance;

      component['age'].set('50');
      component['propertyValue'].set('100000');
      component['gender'].set('male');

      expect(component['isLoading']()).toBe(false);

      component['calculate']();

      expect(component['isLoading']()).toBe(true);

      const req = httpMock.expectOne((request) => {
        return request.url === `${environment.apiUrl}/v1/usufruct-calculations`;
      });

      req.flush({
        originalValue: 100000,
        factor: 0.56,
        calculatedValue: 56000
      });

      setTimeout(() => {
        expect(component['isLoading']()).toBe(false);
        done();
      }, 0);
    });
  });
});
