import { Product, NutriScoreGrade, NutritionInfo, NutrientFlag } from '../../src/core/entities/Product';
import { createMockProduct, createMockNutritionInfo, createMockFlag } from '../mocks/mockData';

describe('Product', () => {
  describe('createMockProduct', () => {
    it('should create a valid product with default values', () => {
      const product = createMockProduct();

      expect(product.barcode).toBe('1234567890123');
      expect(product.name).toBe('Test Product');
      expect(product.brand).toBe('Test Brand');
      expect(product.nutriScoreGrade).toBe('B');
      expect(product.dataSource).toBe('openFoodFacts');
    });

    it('should allow overriding specific fields', () => {
      const product = createMockProduct({
        barcode: '9999999999999',
        name: 'Custom Product',
        nutriScoreGrade: 'A',
      });

      expect(product.barcode).toBe('9999999999999');
      expect(product.name).toBe('Custom Product');
      expect(product.nutriScoreGrade).toBe('A');
      expect(product.brand).toBe('Test Brand');
    });
  });

  describe('NutritionInfo', () => {
    it('should create valid nutrition info with all fields', () => {
      const nutrition = createMockNutritionInfo();

      expect(nutrition.energyKcal).toBe(250);
      expect(nutrition.fat).toBe(12);
      expect(nutrition.saturatedFat).toBe(4);
      expect(nutrition.carbohydrates).toBe(30);
      expect(nutrition.sugars).toBe(15);
      expect(nutrition.fiber).toBe(3);
      expect(nutrition.protein).toBe(8);
      expect(nutrition.salt).toBe(1.2);
      expect(nutrition.sodium).toBe(480);
    });

    it('should allow null values for optional nutrients', () => {
      const nutrition = createMockNutritionInfo({
        fiber: null,
        sodium: null,
      });

      expect(nutrition.fiber).toBeNull();
      expect(nutrition.sodium).toBeNull();
    });
  });

  describe('NutrientFlag', () => {
    it('should create flag with high level', () => {
      const flag = createMockFlag({ level: 'high' });

      expect(flag.level).toBe('high');
    });

    it('should create flag with moderate level', () => {
      const flag = createMockFlag({ level: 'moderate' });

      expect(flag.level).toBe('moderate');
    });

    it('should create flag with low level', () => {
      const flag = createMockFlag({ level: 'low' });

      expect(flag.level).toBe('low');
    });

    it('should include nutrient name and description', () => {
      const flag = createMockFlag({
        nutrient: 'Saturated Fat',
        description: 'Contains high saturated fat',
      });

      expect(flag.nutrient).toBe('Saturated Fat');
      expect(flag.description).toBe('Contains high saturated fat');
    });
  });

  describe('NutriScoreGrade', () => {
    it('should accept all valid grades', () => {
      const validGrades: NutriScoreGrade[] = ['A', 'B', 'C', 'D', 'E', 'Unknown'];

      validGrades.forEach((grade) => {
        const product = createMockProduct({ nutriScoreGrade: grade });
        expect(product.nutriScoreGrade).toBe(grade);
      });
    });
  });

  describe('dataSource', () => {
    it('should accept openFoodFacts as source', () => {
      const product = createMockProduct({ dataSource: 'openFoodFacts' });
      expect(product.dataSource).toBe('openFoodFacts');
    });

    it('should accept userContributed as source', () => {
      const product = createMockProduct({ dataSource: 'userContributed' });
      expect(product.dataSource).toBe('userContributed');
    });

    it('should accept cached as source', () => {
      const product = createMockProduct({ dataSource: 'cached' });
      expect(product.dataSource).toBe('cached');
    });
  });
});
