module.exports = {
  presets: ['module:@react-native/babel-preset'],
  plugins: [
    ['@babel/plugin-proposal-decorators', { legacy: true }],
    ['module-resolver', {
      root: ['./src'],
      extensions: ['.ios.js', '.android.js', '.js', '.ts', '.tsx', '.json'],
      alias: {
        '@core': './src/core',
        '@infrastructure': './src/infrastructure',
        '@application': './src/application',
        '@presentation': './src/presentation',
        '@stores': './src/stores',
        '@i18n': './src/i18n',
        '@di': './src/di',
      },
    }],
  ],
};
