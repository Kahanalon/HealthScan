import { getGradeColor, getGradeDescription, NutriScoreGrade } from '../../src/core/entities/ScoreResult';

describe('ScoreResult', () => {
  describe('getGradeColor', () => {
    it('should return green for grade A', () => {
      expect(getGradeColor('A')).toBe('#038141');
    });

    it('should return light green for grade B', () => {
      expect(getGradeColor('B')).toBe('#85BB2F');
    });

    it('should return yellow for grade C', () => {
      expect(getGradeColor('C')).toBe('#FECB02');
    });

    it('should return orange for grade D', () => {
      expect(getGradeColor('D')).toBe('#EE8100');
    });

    it('should return red for grade E', () => {
      expect(getGradeColor('E')).toBe('#E63E11');
    });

    it('should return gray for Unknown grade', () => {
      expect(getGradeColor('Unknown')).toBe('#808080');
    });

    it('should handle all valid grades', () => {
      const grades: NutriScoreGrade[] = ['A', 'B', 'C', 'D', 'E', 'Unknown'];
      grades.forEach((grade) => {
        const color = getGradeColor(grade);
        expect(color).toBeTruthy();
        expect(color).toMatch(/^#[0-9A-Fa-f]{6}$/);
      });
    });
  });

  describe('getGradeDescription', () => {
    describe('Hebrew locale', () => {
      it('should return Hebrew description for grade A', () => {
        expect(getGradeDescription('A', 'he')).toBe('איכות תזונתית מעולה');
      });

      it('should return Hebrew description for grade B', () => {
        expect(getGradeDescription('B', 'he')).toBe('איכות תזונתית טובה');
      });

      it('should return Hebrew description for grade C', () => {
        expect(getGradeDescription('C', 'he')).toBe('איכות תזונתית בינונית');
      });

      it('should return Hebrew description for grade D', () => {
        expect(getGradeDescription('D', 'he')).toBe('איכות תזונתית נמוכה');
      });

      it('should return Hebrew description for grade E', () => {
        expect(getGradeDescription('E', 'he')).toBe('איכות תזונתית נמוכה מאוד');
      });

      it('should return Hebrew description for Unknown', () => {
        expect(getGradeDescription('Unknown', 'he')).toBe('לא ידוע');
      });
    });

    describe('English locale', () => {
      it('should return English description for grade A', () => {
        expect(getGradeDescription('A', 'en')).toBe('Excellent nutritional quality');
      });

      it('should return English description for grade B', () => {
        expect(getGradeDescription('B', 'en')).toBe('Good nutritional quality');
      });

      it('should return English description for grade C', () => {
        expect(getGradeDescription('C', 'en')).toBe('Average nutritional quality');
      });

      it('should return English description for grade D', () => {
        expect(getGradeDescription('D', 'en')).toBe('Poor nutritional quality');
      });

      it('should return English description for grade E', () => {
        expect(getGradeDescription('E', 'en')).toBe('Bad nutritional quality');
      });

      it('should return English description for Unknown', () => {
        expect(getGradeDescription('Unknown', 'en')).toBe('Unknown');
      });
    });
  });
});
