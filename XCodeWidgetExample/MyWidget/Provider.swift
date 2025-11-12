//
//  Provider.swift
//  XCodeWidgetExample
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

struct Provider: AppIntentTimelineProvider {
    
    // Innitial value, nowhere visible but safe practice to show gallery preview data
    func placeholder(in context: Context) -> SimpleEntry {
        let entry = createPreviewEntry()
        return entry
    }

    // When Widget is first added on screen, or shown in gallery as preview
    func snapshot(for configuration: ConfigurationAppIntent, in context: Context) async -> SimpleEntry {
        let entry = createEntry(for: configuration, in: context)
        return entry
    }
    
    // When Widget is refreshed, this is the main method where data has to be loaded
    func timeline(for configuration: ConfigurationAppIntent, in context: Context) async -> Timeline<SimpleEntry> {
        var entries: [SimpleEntry] = []

        // Normaly you can set multiple data models, to be included in a Timeline to be shown timebased
        // For we only use a single datamodel, so the view only shows that
        
        let entry = createEntry(for: configuration, in: context)
        entries.append(entry)

        return Timeline(entries: entries, policy: .never)
    }
    
    func createEntry(for configuration: ConfigurationAppIntent, in context: Context) -> SimpleEntry {
        if (context.isPreview) {
            return createPreviewEntry()
        }
        
        var message = ""
        
        // not the best mechanism but for works fine for this demo
        // - incomming data is set by Widget, has prio 1
        // - outgoing data is set by app, has prio 2
        // - incomming data is reset as soon as app starts, making outgoing data visible
        var currentCount = SharedStorage().getIncommingDataCount()
        if (currentCount == Int32.min) {
            
            currentCount = SharedStorage().getOutgoingDataCount()
            if (currentCount == Int32.min) {
                currentCount = 0
            }
            else {
                message = "value received from app"
            }
        }

        let entry = SimpleEntry(date: Date(),
                                favoriteConfiguredEmoji: configuration.favoriteEmoji,
                                widgetUrl: "widgetexample://widget?counter=\(currentCount)",
                                count: currentCount,
                                message: message)
        
        return entry
    }
    
    func createPreviewEntry() -> SimpleEntry {
        let entry = SimpleEntry(date: Date(), favoriteConfiguredEmoji: "🤩", widgetUrl: "", count: 5, message: "Preview count is 5")
        return entry
    }

//    func relevances() async -> WidgetRelevances<ConfigurationAppIntent> {
//        // Generate a list containing the contexts this widget is relevant in.
//    }
}
