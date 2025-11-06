//
//  IncrementCounterIntent.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import AppIntents

struct IncrementCounterIntent: AppIntent {
    static var title: LocalizedStringResource { "Increment Counter" }
    static var description: IntentDescription { "Increments the counter by 1" }
    
    func perform() async throws -> some IntentResult {
        
        var currentCount = Int32(0)
        
        let storedValue = SharedStorage().getBestStoredDataCount()
        if (storedValue != Int32.min) {
            currentCount = storedValue
        }
                
        // do action
        let newCount = currentCount + 1
        
        // Save new value
        let userDefaults = UserDefaults(suiteName: Settings.groupId)
        userDefaults?.set(newCount, forKey: Settings.appIncommingDataKey)
        
        // Reload timelines > refreshing widget
        WidgetCenter.shared.reloadTimelines(ofKind: "MyWidget")
        
        // An example how to handle data without opening the app
        do {
            try await SilentNotificationService().sendDataWithoutOpeningApp()
        } catch {
            print("Error occurred: \(error)")
        }
        
        return .result()
    }
}
