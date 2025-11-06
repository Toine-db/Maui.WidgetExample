//
//  MyWidget.swift
//  MyWidget
//
//  Created by Toine Boer on 05/11/2025.
//

import WidgetKit
import SwiftUI

struct MyWidget: Widget {
    let kind: String = "MyWidget"

    var body: some WidgetConfiguration {
        AppIntentConfiguration(kind: kind, intent: ConfigurationAppIntent.self, provider: Provider()) { entry in
            MyWidgetEntryView(entry: entry)
                .containerBackground(.fill.tertiary, for: .widget)
        }
        .supportedFamilies([.systemMedium, .systemLarge]) // Widget sizes that you support
    }
}

#Preview(as: .systemSmall) {
    MyWidget()
} timeline: {
    SimpleEntry(date: .now, favoriteConfiguredEmoji: ConfigurationAppIntent.smiley, count: 4, message: "preview design")
    SimpleEntry(date: .now, favoriteConfiguredEmoji: ConfigurationAppIntent.starEyes, count: 3, message: "preview design")
}
