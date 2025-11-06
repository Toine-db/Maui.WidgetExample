//
//  SharedStorage.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

class SharedStorage {
    
    func getIncommingDataCount() -> Int32 {
                
        let userDefaults = UserDefaults(suiteName: Settings.groupId)
               
        // Check if value is valid (not nil)
        let appIncommingData = userDefaults?.integer(forKey: Settings.appIncommingDataKey)
        if let appIncommingDataCount = appIncommingData {
            return Int32(appIncommingDataCount)
        }
        
        return Int32.min
    }
    
    func getOutgoingDataCount() -> Int32 {
                
        let userDefaults = UserDefaults(suiteName: Settings.groupId)
               
        // Check if value is valid (not nil)
        let appOutgoingData = userDefaults?.integer(forKey: Settings.appOutgoingDataKey)
        if let appOutgoingDataCount = appOutgoingData {
            return Int32(appOutgoingDataCount)
        }
        
        return Int32.min
    }
    
    func getBestStoredDataCount() -> Int32 {
        var currentCount = SharedStorage().getIncommingDataCount()
        if (currentCount == Int32.min) {
            currentCount = SharedStorage().getOutgoingDataCount()
        }
        
        return currentCount;
    }
}
