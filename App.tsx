import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import RootNavigator from './RootNavigator';
import Provider from './context'; 
export default function App() {
  return (
    <Provider>
    <NavigationContainer>
      <RootNavigator />
    </NavigationContainer>
    </Provider>
  );
}
