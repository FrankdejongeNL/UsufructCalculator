import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { UsufructService } from './usufruct.service';
import { Gender, CalculationMethod } from '../models/usufruct.models';
import { environment } from '../../../environments/environment';

describe('UsufructService', () => {
  let service: UsufructService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        UsufructService
      ]
    });
    service = TestBed.inject(UsufructService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should make GET request to correct URL with query parameters', () => {
    const request = {
      amount: 100000,
      age: 50,
      gender: Gender.Male,
      calculationMethod: CalculationMethod.EenLeven
    };

    const mockResponse = {
      originalValue: 100000,
      factor: 0.5,
      calculatedValue: 50000
    };

    service.calculate(request).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne((req) => {
      return req.url === `${environment.apiUrl}/v1/usufruct-calculations`;
    });

    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('amount')).toBe('100000');
    expect(req.request.params.get('age')).toBe('50');
    expect(req.request.params.get('gender')).toBe('0');
    expect(req.request.params.get('calculationMethod')).toBe('0');

    req.flush(mockResponse);
  });

  it('should handle female gender correctly', () => {
    const request = {
      amount: 200000,
      age: 60,
      gender: Gender.Female,
      calculationMethod: CalculationMethod.EenLeven
    };

    service.calculate(request).subscribe();

    const req = httpMock.expectOne((req) => {
      return req.url === `${environment.apiUrl}/v1/usufruct-calculations`;
    });

    expect(req.request.params.get('gender')).toBe('1');

    req.flush({
      originalValue: 200000,
      factor: 0.6,
      calculatedValue: 120000
    });
  });

  it('should handle HTTP errors', () => {
    const request = {
      amount: 100000,
      age: 50,
      gender: Gender.Male,
      calculationMethod: CalculationMethod.EenLeven
    };

    service.calculate(request).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        expect(error.status).toBe(500);
      }
    });

    const req = httpMock.expectOne((req) => {
      return req.url === `${environment.apiUrl}/v1/usufruct-calculations`;
    });

    req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });
  });
});
